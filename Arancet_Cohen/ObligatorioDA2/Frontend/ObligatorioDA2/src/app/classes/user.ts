export class User {
    username: string;
    name: string;
    email: string;
    isAdmin: boolean;
    surname: string;

    constructor(username:string, name:string, surname:string,email:string, isAdmin:boolean){
        this.name = name;
        this.surname = surname;
        this.username = username;
        this.email = email;
        this.isAdmin =isAdmin;
    }

}