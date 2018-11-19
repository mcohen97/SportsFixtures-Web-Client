export class Team {
    id:number;
    name: string;
    sportName:string
    photo:string;
    points:number;

    public static NO_PHOTO_DIR = "";

    public static getClone(aTeam:Team): Team{
        var newTeam = new Team(aTeam.name, aTeam.sportName);
        newTeam.id = aTeam.id;
        newTeam.photo = aTeam.photo;
        return newTeam;
    }
    
    constructor(name:string, sportName:string){
        this.id = 0;
        this.name = name;
        this.sportName = sportName;
        this.photo = Team.NO_PHOTO_DIR;
    }

    public toString = () : string => {
        return this.name;
    }

}