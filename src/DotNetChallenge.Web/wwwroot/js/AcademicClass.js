import { HttpCancelPreviousRequestService } from './httpClient/HttpCancelPreviousRequest.js';

export class AcademicClassService {
    constructor() {
        this.httpService = new HttpCancelPreviousRequestService();
        this.baseUrl = '/api/v1/AcademicClass/';
    }

    async deleteClass(classId) {
        const url = `${this.baseUrl}Delete/${classId}`;
        try {
            const response = await this.httpService.delete(url);
            const responseData = await response.json();
            if (response.StatusCode === 200) {
                console.log(`Classe acadêmica com ID ${classId} excluída com sucesso.`);
                document.getElementById(`class-row-${classId}`).remove();
            } else {
                console.error(`Erro ao excluir a classe com ID ${classId}: ${response.Error}`);
            }
        } catch (error) {
            console.error(`Erro na requisição de exclusão: ${error.message}`);
        }
    }

    async createClass(academicClass) {
        const url = `${this.baseUrl}CreateAcademicClass`;
        try {
            const response = await this.httpService.post(url, academicClass);

            if (response.StatusCode === 200) {
                console.log("Classe acadêmica criada com sucesso.");
                return response.Data;
            } else {
                console.error(`Erro ao criar a classe acadêmica: ${response.Error}`);
            }
        } catch (error) {
            console.error(`Erro na requisição de criação: ${error.message}`);
        }
    }

    async updateClass(academicClass) {
        const url = `${this.baseUrl}UpdateAcademicClass`;
        try {
            const response = await this.httpService.put(url, academicClass);
            const responseData = await response.json();
            if (response.StatusCode === 200) {
                console.log("Classe acadêmica atualizada com sucesso.");
            } else {
                console.error(`Erro ao atualizar a classe acadêmica: ${response.Error}`);
            }
        } catch (error) {
            console.error(`Erro na requisição de atualização: ${error.message}`);
        }
    }

    async getClassDetails(classId) {
        const url = `${this.baseUrl}Details/${classId}`;
        try {
            const response = await this.httpService.get(url);
            if (response.StatusCode === 200) {
                console.log("Detalhes da classe acadêmica obtidos com sucesso.", response.Data);
                return response.Data;
            } else {
                console.error(`Erro ao obter os detalhes da classe com ID ${classId}: ${response.Error}`);
            }
        } catch (error) {
            console.error(`Erro na requisição de obtenção de detalhes: ${error.message}`);
        }
    }

    async getAllClasses() {
        try {
            const response = await this.httpService.get(this.baseUrl);
            if (response.StatusCode === 200) {
                console.log("Lista de classes acadêmicas obtida com sucesso.");
                return response.Data;
            } else {
                console.error(`Erro ao obter a lista de classes acadêmicas: ${response.Error}`);
            }
        } catch (error) {
            console.error(`Erro na requisição de obtenção de classes: ${error.message}`);
        }
    }
}

window.academicClassService = new AcademicClassService();