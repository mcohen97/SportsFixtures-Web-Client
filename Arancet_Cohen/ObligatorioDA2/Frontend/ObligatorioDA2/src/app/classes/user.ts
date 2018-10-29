export class User {
    username: string;
    password: string;
    name: string;
    email: string;
    isAdmin: boolean;
    surname: string;

    constructor(username:string, password:string, name:string, surname:string,email:string, isAdmin:boolean){
        this.name = name;
        this.surname = surname;
        this.username = username;
        this.password = password;
        this.email = email;
        this.isAdmin =isAdmin;
    }

}