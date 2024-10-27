import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class AcademicClassModal {
    constructor(modalId, saveButtonId, formId) {
        this.modal = $(`#${modalId}`);
        this.saveButton = $(`#${saveButtonId}`);
        this.form = $(`#${formId}`);
        this.yearInput = this.form.find("#year");
        this.classNameInput = this.form.find("#className");
        this.modalFooter = this.modal.find(".modal-footer");
        this.httpService = new HttpCancelPreviousRequestService();
        this.baseUrl = '/AcademicClass/';

        this.initializeEvents();
        this.initializePopovers();
    }

    initializeEvents() {
        this.saveButton.on("click", () => this.handleSave());
    }

    initializePopovers() {
        document.querySelectorAll('[data-bs-toggle="popover"]').forEach(popoverTriggerEl => {
            new bootstrap.Popover(popoverTriggerEl, {
                html: true,
                placement: 'top',
                sanitize: false,
                trigger: 'focus',
                content: () => `
                    <div style="text-align: center;">
                        <p>Tem certeza que deseja excluir esta classe?</p>
                        <button class="btn btn-sm btn-danger" onclick="academicClassModal.deleteClass('${popoverTriggerEl.id}')">Sim, Excluir</button>
                        <button class="btn btn-sm btn-secondary" onclick="academicClassModal.cancelDelete('${popoverTriggerEl.id}')">Cancelar</button>
                    </div>
                `
            });
        });
    }

    openModal() {
        this.form.trigger("reset");
        this.hideErrorAlert();
        this.hideSuccessAlert();
        this.form.removeClass("d-none"); 
        this.modalFooter.removeClass("d-none");
        this.modal.modal("show");
    }

    openEditModal(className, year, classId) {
        this.classNameInput.val(className);
        this.yearInput.val(year);
        this.classId = classId;

        this.saveButton.text("Atualizar");
        this.modal.find(".modal-title").text("Editar Classe Acadêmica");

        this.modal.modal("show");
    }

    async handleSave() {
        const className = this.classNameInput.val();
        const year = parseInt(this.yearInput.val());

        const formData = { ClassName: className, Year: year };

        try {
            if (this.classId) {
                formData.Id = this.classId;
                await this.updateAcademicClass(formData);
            } else {
                await this.createAcademicClass(formData);
            }
        } catch (error) {
            this.showErrorAlert("Erro ao salvar a classe acadêmica.");
            console.error(error);
        }
    }

    async createAcademicClass(academicClass) {
        try {
            const response = await this.httpService.post(`${this.baseUrl}CreateAcademicClass`, academicClass);

            if (response.StatusCode == 200) {
                this.showSuccessAlert("Classe criada com sucesso!");
                setTimeout(() => {
                    this.modal.modal("hide");
                    location.reload();
                }, 2000);
                return await response.Result;
            }
        } catch (error) {

            if (error.message === "DUPLICATE_CLASS_NAME") {
                this.showErrorAlert("Erro: Já existe uma turma com esse nome. Por favor, escolha outro nome.");
                return null;
            }
            this.showErrorAlert(`Erro ao criar a classe acadêmica: ${error.message}`);
        }
    }

    async updateAcademicClass(academicClass) {
        try {
            `${this.baseUrl}UpdateAcademicClass`
            const response = await this.httpService.put(`${this.baseUrl}UpdateAcademicClass`, academicClass);

            if (response.StatusCode == 200) {
                this.showSuccessAlert("Classe atualizada com sucesso!");
                setTimeout(() => {
                    this.modal.modal("hide");
                    location.reload();
                }, 2000);
                return await response.Result;
            }
        } catch (error) {
            this.showErrorAlert(error.message);
        }
    }

    async deleteClass(classId) {
        try {
            const response = await this.httpService.delete(`${this.baseUrl}${classId}`);
            if (response.StatusCode === 200) {
                document.getElementById(`class-row-${classId}`).remove();
                this.showGlobalSuccessAlert("Classe excluída com sucesso!");
            } else {
                this.showErrorAlert("Erro ao excluir a classe.");
            }
        } catch (error) {
            this.showErrorAlert(`Erro na requisição de exclusão: ${error.message}`);
        }
    }

    showErrorAlert(message) {
        const errorAlert = document.getElementById("errorAlert");
        const errorAlertMessage = document.getElementById("errorAlertMessage");
        errorAlertMessage.textContent = message;
        errorAlert.classList.remove("d-none");
    }

    showSuccessAlert(message) {
        const successAlert = document.getElementById("successAlert");
        const successAlertMessage = document.getElementById("successAlertMessage");
        successAlertMessage.textContent = message;
        successAlert.classList.remove("d-none");
        this.form.addClass("d-none");
        this.modalFooter.addClass("d-none")
    }

    showGlobalSuccessAlert(message) {
        const successAlert = document.getElementById("globalSuccessAlert");
        const successAlertMessage = document.getElementById("globalSuccessAlertMessage");
        successAlertMessage.textContent = message;
        successAlert.classList.remove("d-none");

        setTimeout(() => {
            successAlert.classList.add("d-none");
        }, 3000);
    }

    hideSuccessAlert() {
        const successAlert = document.getElementById("successAlert");
        successAlert.classList.add("d-none");
        this.form.removeClass("d-none");
        this.modalFooter.removeClass("d-none");
    }

    hideErrorAlert() {
        const errorAlert = document.getElementById("errorAlert");
        errorAlert.classList.add("d-none");
    }

    confirmDelete(classId) {
        const deleteButton = document.getElementById(`delete-${classId}`);
        const popover = bootstrap.Popover.getInstance(deleteButton) || new bootstrap.Popover(deleteButton);
        popover.toggle();
    }

    cancelDelete(buttonId) {
        const deleteButton = document.getElementById(buttonId);
        const popover = bootstrap.Popover.getInstance(deleteButton);
        if (popover) popover.hide();
    }
}

const academicClassModal = new AcademicClassModal("createClassModal", "saveClassButton", "createClassForm");
window.academicClassModal = academicClassModal;
window.openEditModal = (className, year, classId) => academicClassModal.openEditModal(className, year, classId);
document.getElementById("openCreateClassModal").addEventListener("click", () => academicClassModal.openModal());