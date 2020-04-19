export class Standing {
    teamId: number;
    position: number;
    points:number;
    
    public static getClone(aStanding:Standing): Standing{   
        var newStanding = new Standing();
        newStanding.points = aStanding.points;
        newStanding.position = aStanding.position;
        newStanding.teamId = aStanding.teamId;
        return newStanding;
    }
    
    constructor(){
        this.teamId = 0;
        this.position = 0;
        this.points = 0;
    }

    public static newStanding(teamId:number, position:number):Standing{
        var newStanding = new Standing();
        newStanding.teamId = teamId;
        newStanding.position = position;
        return newStanding;
    }

    public toString = () : string => {
        return "team: " + this.teamId + " got " + (this.position?this.position:this.points) + " position";
    }

}