export class ErrorResponse {
    errorMessage: string;
    errorCode: number;
    errorObject: any
    constructor(){
        this.errorMessage = "";
        this.errorCode = -1;
        this.errorObject = undefined;
    }
}