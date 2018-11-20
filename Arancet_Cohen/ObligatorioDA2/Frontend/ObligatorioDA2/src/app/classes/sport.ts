export class Sport {
    name: string;
    isTwoTeams: boolean;

    public static getClone(aSport:Sport): Sport{
        var newSport = new Sport(aSport.name);
        newSport.isTwoTeams = aSport.isTwoTeams;
        return newSport;
    }
    
    constructor(name:string){
        this.name = name;
        this.isTwoTeams = true;
    }

}