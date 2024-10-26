import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class StudentModal {
    constructor() {
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

        if (!this.modalElement || !this.form || !this.nameInput || !this.userInput || !this.saveButton || !this.modalTitle || !this.passwordInput || !this.confirmPasswordInput) {
            console.error("Erro ao inicializar elementos do modal. Verifique se os IDs estão corretos na view.");
            return;
        }

        this.form.addEventListener("input", () => this.toggleSubmitButton());
        this.modalElement.addEventListener("hidden.bs.modal", () => this.resetForm());
    }

    openStudentModal(student = null) {
        this.isEditMode = student?.Id != null;
        this.student = student || {};
        this.setModalData();
        this.passwordFields.style.display = this.isEditMode ? 'none' : 'block';
        const bootstrapModal = new bootstrap.Modal(this.modalElement);
        bootstrapModal.show();
    }

    setModalData() {
        this.modalTitle.textContent = this.isEditMode ? "Editar Estudante" : "Adicionar Estudante";

        this.nameInput.value = this.isEditMode ? this.student.Name : '';
        this.userInput.value = this.isEditMode ? this.student.User : '';
        this.passwordInput.value = '';
        this.confirmPasswordInput.value = '';

        this.toggleSubmitButton();
    }

    toggleSubmitButton() {
        const allFilled = [...this.form.querySelectorAll("input[required]")].every(input => input.value.trim() !== "");
        this.saveButton.disabled = !allFilled;
    }

    resetForm() {
        this.form.reset();
        this.saveButton.disabled = true;
        this.passwordFeedback.textContent = "";
        this.confirmPasswordFeedback.textContent = "";
    }

    validateAndAddStudent() {
        const passwordInput = document.getElementById("password");
        const confirmPasswordInput = document.getElementById("confirmPassword");
        const passwordFeedback = document.getElementById("passwordFeedback");
        const confirmPasswordFeedback = document.getElementById("confirmPasswordFeedback");
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;

        const passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

        let isValid = true;

        passwordFeedback.textContent = "";
        confirmPasswordFeedback.textContent = "";
        passwordInput.classList.remove("is-invalid");
        confirmPasswordInput.classList.remove("is-invalid");

        if (!passwordPattern.test(password)) {
            passwordFeedback.textContent = "A senha deve ter pelo menos 8 caracteres, incluindo uma letra maiúscula, uma minúscula, um número e um caractere especial.";
            passwordInput.classList.add("is-invalid");
            isValid = false;
        }

        if (password !== confirmPassword) {
            confirmPasswordFeedback.textContent = "As senhas não coincidem.";
            confirmPasswordInput.classList.add("is-invalid");
            isValid = false;
        }

        if (isValid) {
            this.addStudent();
        }
    }

    async addStudent() {
        const name = document.getElementById("studentName").value;
        const user = document.getElementById("studentUser").value;
        const password = document.getElementById("password").value;

        const studentDto = { Name: name, User: user, Password: password };

        try {
            const response = await this.httpService.post('/CreateStudent', studentDto);

            if (response.ok) {
                const successAlert = document.getElementById("successAlert");
                const footer = document.getElementById("modalFooter");
                successAlert.classList.remove("d-none");

                this.form.classList.add("d-none");
                footer.classList.add("d-none");

                setTimeout(() => {
                    successAlert.classList.add("d-none");
                    this.form.classList.remove("d-none");
                    footer.classList.remove("d-none");
                    const modal = bootstrap.Modal.getInstance(this.modalElement);
                    modal.hide();

                    this.resetForm();
                }, 3000);
            } else {
                console.error("Erro ao cadastrar o estudante.");
            }
        } catch (error) {
            console.error("Erro na requisição:", error);
        }
    }
}

window.studentModalService = new StudentModal();