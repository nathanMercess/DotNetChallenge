import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class StudentModal {
    constructor() {
        this.initializeElements();
        this.initializeEventListeners();
    }

    initializeElements() {
        this.httpService = new HttpCancelPreviousRequestService();
        this.modalElement = document.getElementById("studentModal");
        this.form = document.getElementById("studentForm");
        this.nameInput = document.getElementById("studentName");
        this.userInput = document.getElementById("studentUser");
        this.passwordInput = document.getElementById("password");
        this.confirmPasswordInput = document.getElementById("confirmPassword");
        this.passwordFeedback = document.getElementById("passwordFeedback");
        this.confirmPasswordFeedback = document.getElementById("confirmPasswordFeedback");
        this.saveButton = document.getElementById("saveStudentButton");
        this.modalTitle = document.getElementById("studentModalLabel");
        this.passwordFields = document.getElementById("passwordFields");
        this.successMessage = document.getElementById("successMessage");
        this.successAlert = document.getElementById("successAlert");
        this.modalFooter = document.getElementById("modalFooter");

        if (!this.modalElement || !this.form || !this.nameInput || !this.userInput || !this.saveButton || !this.modalTitle || !this.passwordInput || !this.confirmPasswordInput) {
            console.error("Erro ao inicializar elementos do modal. Verifique se os IDs estão corretos na view.");
            return;
        }
    }

    initializeEventListeners() {
        this.form.addEventListener("input", () => this.toggleSubmitButton());
        this.modalElement.addEventListener("hidden.bs.modal", () => this.resetForm());
    }

    openStudentModal(student = null) {
        this.isEditMode = student?.Id != null;
        this.student = student || {};
        this.setupModalData();
        this.passwordFields.style.display = this.isEditMode ? 'none' : 'block';
        new bootstrap.Modal(this.modalElement).show();
    }

    setupModalData() {
        this.modalTitle.textContent = this.isEditMode ? "Editar Estudante" : "Adicionar Estudante";
        this.saveButton.onclick = this.isEditMode ? () => this.updateStudent() : () => this.validateAndAddStudent();
        this.setFormValues();
    }

    setFormValues() {
        this.nameInput.value = this.isEditMode ? this.student.Name : '';
        this.userInput.value = this.isEditMode ? this.student.User : '';
        this.passwordInput.value = '';
        this.confirmPasswordInput.value = '';
        if (this.isEditMode) {
            this.nameInput.addEventListener('input', () => this.validateEditChanges());
            this.userInput.addEventListener('input', () => this.validateEditChanges());
        } else {
            this.nameInput.addEventListener('input', () => this.validateAddChanges());
            this.userInput.addEventListener('input', () => this.validateAddChanges());
            this.passwordInput.addEventListener('input', () => this.validateAddChanges());
            this.confirmPasswordInput.addEventListener('input', () => this.validateAddChanges());
        }
    }

    validateEditChanges() {
        const nameChanged = this.nameInput.value.trim() !== "" && this.nameInput.value !== this.student.Name;
        const userChanged = this.userInput.value.trim() !== "" && this.userInput.value !== this.student.User;
        this.saveButton.disabled = !(nameChanged || userChanged);
    }

    validateAddChanges() {
        const nameFilled = this.nameInput.value.trim() !== "";
        const userFilled = this.userInput.value.trim() !== "";
        const passwordsMatch = this.passwordInput.value === this.confirmPasswordInput.value;
        const passwordsFilled = this.passwordInput.value.trim() !== "" && this.confirmPasswordInput.value.trim() !== "";

        this.saveButton.disabled = !(nameFilled && userFilled && passwordsFilled && passwordsMatch);
    }

    resetForm() {
        this.form.reset();
        this.saveButton.disabled = true;
        this.clearFeedbackMessages();
    }

    clearFeedbackMessages() {
        this.passwordFeedback.textContent = "";
        this.confirmPasswordFeedback.textContent = "";
    }

    validateAndAddStudent() {
        if (this.isPasswordValid()) this.addStudent();
    }

    isPasswordValid() {
        const password = this.passwordInput.value;
        const confirmPassword = this.confirmPasswordInput.value;
        const passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

        this.clearFeedbackMessages();
        this.toggleInvalidClass(this.passwordInput, passwordPattern.test(password), "A senha deve ter pelo menos 8 caracteres, incluindo uma letra maiúscula, uma minúscula, um número e um caractere especial.");
        this.toggleInvalidClass(this.confirmPasswordInput, password === confirmPassword, "As senhas não coincidem.");

        return passwordPattern.test(password) && password === confirmPassword;
    }

    toggleInvalidClass(input, condition, message) {
        if (!condition) {
            input.nextElementSibling.textContent = message;
            input.classList.add("is-invalid");
        } else {
            input.classList.remove("is-invalid");
        }
    }

    async addStudent() {
        const studentDto = {
            Name: this.nameInput.value,
            User: this.userInput.value,
            Password: this.passwordInput.value
        };
        await this.saveStudent('/CreateStudent', studentDto, "O estudante foi cadastrado com sucesso!");
    }

    async updateStudent() {
        const studentDto = {
            Id: this.student.Id,
            Name: this.nameInput.value,
            User: this.userInput.value
        };
        await this.saveStudent('/UpdateStudent', studentDto, "O estudante foi atualizado com sucesso!", "put");
    }

    async saveStudent(endpoint, studentDto, successMessage, method = "post") {
        try {
            const response = method === "put"
                ? await this.httpService.put(endpoint, studentDto)
                : await this.httpService.post(endpoint, studentDto);

            if (response.StatusCode === 200) {
                this.showSuccessMessage(successMessage);
                setTimeout(() => this.closeModalAndReload(), 3000);
            } else {
                alert(`Erro ao cadastrar/atualizar o estudante: ${response.Error}`);
            }
        } catch (error) {
            console.error("Erro na requisição:", error);
            alert("Erro ao processar a requisição.");
        }
    }

    showSuccessMessage(message) {
        this.successAlert.classList.remove("d-none");
        this.successMessage.textContent = message;
        this.form.classList.add("d-none");
        this.modalFooter.classList.add("d-none");
    }

    closeModalAndReload() {
        this.successAlert.classList.add("d-none");
        this.form.classList.remove("d-none");
        this.modalFooter.classList.remove("d-none");
        bootstrap.Modal.getInstance(this.modalElement).hide();
        this.resetForm();
        location.reload();
    }
}

window.studentModalService = new StudentModal();