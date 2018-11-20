export class Team {
    id:number;
    name: string;
    sportName:string
    photo:string;
    points:number;
    followed:boolean //Used for followed teams table only

    public static getClone(aTeam:Team): Team{
        var newTeam = new Team(aTeam.name, aTeam.sportName);
        newTeam.id = aTeam.id;
        newTeam.photo = aTeam.photo;
        newTeam.followed = aTeam.followed;
        return newTeam;
    }
    
    constructor(name:string, sportName:string){
        this.id = 0;
        this.name = name;
        this.sportName = sportName;
        this.photo = "";
        this.followed = false;
    }

    public toString = () : string => {
        return this.name;
    }

}