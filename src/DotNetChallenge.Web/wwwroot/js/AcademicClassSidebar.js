import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class AcademicClassOffcanvas {
    constructor() {
        this.httpService = new HttpCancelPreviousRequestService();
        this.studentListContainer = document.getElementById("studentList");

        const offcanvasElement = document.getElementById("offcanvasStudentList");
        offcanvasElement.addEventListener("show.bs.offcanvas", () => this.loadAvailableStudents());
    }

    async loadAvailableStudents() {
        try {
            const response = await this.httpService.get('/GetAllActiveStudents');
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
                        <button class="btn btn-primary btn-sm" onclick="academicClassOffcanvas.addStudentToClass('${student.id}')">Adicionar</button>
                    </div> `;

                this.studentListContainer.appendChild(studentItem);
            });

        } catch (error) {
            this.showErrorAlert("Erro ao carregar a lista de estudantes. Por favor, tente novamente.");
        }
    }

    async addStudentToClass(studentId) {
        const classId = this.getClassId();

        try {
            const response = await this.httpService.post(`/api/academic-class/${classId}/add-student`, { studentId });
            if (response.ok) {
                this.showSuccessAlert("Estudante adicionado com sucesso!");
                this.loadAvailableStudents();
            } else {
                this.showErrorAlert("Erro ao adicionar estudante.");
            }
        } catch (error) {
            this.showErrorAlert("Erro na conexão. Por favor, tente novamente.");
        }
    }

    getClassId() {
        const urlParts = window.location.pathname.split('/');
        return urlParts[urlParts.length - 1];
    }

    showSuccessAlert(message) {
        alert(message);
    }

    showErrorAlert(message) {
        alert(message);
    }
}

const academicClassOffcanvas = new AcademicClassOffcanvas();
window.academicClassOffcanvas = academicClassOffcanvas;
