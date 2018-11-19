export class Standing {
    teamId: number;
    position: number;
    
    public static getClone(aStanding:Standing): Standing{   
        return new Standing(aStanding.teamId, aStanding.position);
    }
    
    constructor(teamId:number, position:number){
        this.teamId = teamId;
        this.position = position;
    }

}