import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class AcademicClassOffcanvas {
    constructor() {
        this.httpService = new HttpCancelPreviousRequestService();
        this.studentListContainer = document.getElementById("studentList");
        const offcanvasElement = document.getElementById("offcanvasStudentList");
        offcanvasElement.addEventListener("show.bs.offcanvas", () => this.loadAvailableStudents());
        this.baseUrl = '/AcademicClass/';
    }

    async loadAvailableStudents() {
        this.hideErrorAlert();
        try {
            const response = await this.httpService.get(`/GetActiveStudentsNotInCourse/${this.getClassId()}`);
            const students = response.Data;

            this.studentListContainer.innerHTML = '';

            students.forEach(student => {
                const studentItem = document.createElement("div");
                studentItem.classList.add("card", "mb-3", "p-3", "student-card");

                studentItem.innerHTML = `
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h5 class="mb-0">${student.name}</h5>
                            <small class="text-muted">(${student.user})</small>
                        </div>
                        <button class="btn btn-primary btn-sm" onclick="academicClassOffcanvas.addStudentToClass('${student.id}', '${encodeURIComponent(student.name)}')">Adicionar</button>
                    </div> `;

                this.studentListContainer.appendChild(studentItem);
            });

            if (students.length === 0) {
                const noStudentsMessage = document.createElement("div");
                noStudentsMessage.classList.add("text-center", "text-muted", "mt-3");
                noStudentsMessage.textContent = "Nenhum aluno disponível para adicionar.";

                this.studentListContainer.appendChild(noStudentsMessage);
            }

        } catch (error) {
            this.showErrorAlert("Erro ao carregar a lista de estudantes. Por favor, tente novamente.");
        }
    }

    async addStudentToClass(studentId, studentName) {
        const classId = this.getClassId();
        const url = `${this.baseUrl}AddStudentToClass/${classId}/${studentId}`;
        try {
            const response = await this.httpService.post(url, { studentId });
            if (response.StatusCode == 200) {
                this.showSuccessAlert(`${studentName} adicionado(a) com sucesso à classe!`);
                this.loadAvailableStudents();
            } else {
                this.showErrorAlert("Erro ao adicionar estudante.");
            }
        } catch (error) {
            this.showErrorAlert("Erro na conexão. Por favor, tente novamente.");
        }
    }

    async removeStudentFromClass(studentId, studentName) {
        const classId = this.getClassId();
        try {
            const response = await this.httpService.delete(`${this.baseUrl}RemoveStudentFromClass/${classId}/${studentId}`);
            if (response.StatusCode == 200) {
                document.getElementById(`student-card-${studentId}`).remove();
                this.showSuccessAlertGlobal(`Estudante ${decodeURIComponent(studentName)} removido com sucesso!`);
            } else {
                this.showErrorAlert("Erro ao remover o estudante da classe.");
            }
        } catch (error) {
            this.showErrorAlert("Erro na requisição de remoção.");
        }
    }

    getClassId() {
        const urlParts = window.location.pathname.split('/');
        return urlParts[urlParts.length - 1];
    }

    showSuccessAlert(message) {
        const successAlert = document.getElementById("sidebarSuccessAlert");
        const successAlertMessage = document.getElementById("sidebarSuccessAlertMessage");
        successAlertMessage.textContent = message;
        successAlert.classList.remove("d-none");

        setTimeout(() => {
            successAlert.classList.add("d-none");
        }, 3000);
    }

    showSuccessAlertGlobal(message) {
        const alertContainer = document.getElementById("globalSuccessAlert");
        const alertMessage = document.getElementById("globalSuccessAlertMessage");

        alertMessage.textContent = message;
        alertContainer.classList.remove("d-none");

        setTimeout(() => {
            alertContainer.classList.add("d-none");
        }, 3000);
    }


    showErrorAlert(message) {
        const errorAlert = document.getElementById("studentListError");
        errorAlert.textContent = message;
        errorAlert.classList.remove("d-none");
    }

    hideErrorAlert() {
        const errorAlert = document.getElementById("studentListError");
        errorAlert.classList.add("d-none");
    }
}

const academicClassOffcanvas = new AcademicClassOffcanvas();
window.academicClassOffcanvas = academicClassOffcanvas;

document.addEventListener('DOMContentLoaded', () => {
    const offcanvasElement = document.getElementById('offcanvasStudentList');

    offcanvasElement.addEventListener('hidden.bs.offcanvas', () => {
        location.reload();
    });
});