export class Comment{
    id:number;
    text:string;
    makerUsername:string;

    constructor(text:string){
        this.id = 0;
        this.text = text;
        this.makerUsername = "";
    }

}