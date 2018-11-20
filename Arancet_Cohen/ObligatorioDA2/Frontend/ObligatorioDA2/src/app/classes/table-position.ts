export class TablePosition{
    teamId: number;
    points: number;
    
    public static getClone(aTablePosition:TablePosition): TablePosition{   
        return new TablePosition(aTablePosition.teamId, aTablePosition.points);
    }
    
    constructor(teamId:number, position:number){
        this.teamId = teamId;
        this.points = position;
    }

}