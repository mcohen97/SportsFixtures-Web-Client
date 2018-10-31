export class User {
    username: string;
    password: string;
    name: string;
    email: string;
    isAdmin: boolean;
    surname: string;

    public static getClone(aUser:User): User{
        var user = new User(aUser.username, aUser.name, aUser.surname, aUser.email);
        user.password = aUser.password;
        return user;
    }
    
    constructor(username:string, name:string, surname:string,email:string){
        this.name = name;
        this.surname = surname;
        this.username = username;
        this.email = email;
        this.isAdmin = false;
        this.password = "aPassword";
    }

}