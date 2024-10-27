import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class StudentService {
    constructor() {
        this.httpService = new HttpCancelPreviousRequestService();
        this.form = document.getElementById("addStudentForm");
        this.submitButton = document.getElementById("submitButton");
        this.modalElement = document.getElementById("addStudentModal");

        document.addEventListener('DOMContentLoaded', () => this.initializePopover());
    }

    searchStudents() {
        const searchText = document.getElementById("studentSearch").value.toLowerCase();
        const rows = document.querySelectorAll("#studentTableBody tr");

        rows.forEach(row => {
            const name = row.cells[0].innerText.toLowerCase();
            const user = row.cells[1].innerText.toLowerCase();
            row.style.display = (name.includes(searchText) || user.includes(searchText)) ? "" : "none";
        });
    }

    async deleteStudent(studentId) {
        try {
            const response = await this.httpService.delete(`${studentId}`);

            if (response.StatusCode === 200) {
                document.getElementById(`student-row-${studentId}`).remove();
                this.showGlobalSuccessAlert("Excluída com sucesso!");
                this.cancelDelete(`deleteButton-${studentId}`);
            } else {
                console.error(`Erro ao excluir o estudante: ${response.Error}`);
            }
        } catch (error) {
            console.error(`Erro na requisição de exclusão: ${error.message}`);
        }
    }

    initializePopover() {
        const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
        popoverTriggerList.forEach(popoverTriggerEl => {
            new bootstrap.Popover(popoverTriggerEl, {
                html: true,
                placement: 'top',
                sanitize: false,
                trigger: 'focus',
                content: () => `
                    <div style='text-align: center;'>
                        <p>Tem certeza que deseja excluir este aluno?</p>
                        <button class='btn btn-sm btn-danger' onclick='studentService.deleteStudent("${popoverTriggerEl.id}")'>Sim, Excluir</button>
                        <button class='btn btn-sm btn-secondary' onclick='studentService.cancelDelete("${popoverTriggerEl.id}")'>Cancelar</button>
                    </div>
                `
            });
        });
    }

    confirmDelete(studentId) {
        const deleteButton = document.getElementById(studentId);
        const popover = bootstrap.Popover.getInstance(deleteButton) || new bootstrap.Popover(deleteButton);
        popover.toggle();
    }

    cancelDelete(buttonId) {
        const deleteButton = document.getElementById(buttonId);
        const popover = bootstrap.Popover.getInstance(deleteButton);
        if (popover) popover.hide();
    }

    goToStudentDetails(studentId) {
        window.location.href = `/StudentDetails/${studentId}`;
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
}

window.studentService = new StudentService();
