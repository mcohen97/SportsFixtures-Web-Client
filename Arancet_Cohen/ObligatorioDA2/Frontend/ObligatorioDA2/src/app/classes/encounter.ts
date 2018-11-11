import { Team } from "./team";

export class Encounter {
    id: number;
    sportName: string;
    hasResult: boolean;
    date: Date;
    teamIds: Array<number>;
    commentsids: Array<number>;
    teams:Array<Team>;

    public static getClone(aEncounter:Encounter): Encounter{
        var encounter = new Encounter(aEncounter.sportName, aEncounter.hasResult, aEncounter.date);
        encounter.teamIds = aEncounter.teamIds;
        encounter.commentsids = aEncounter.commentsids;
        encounter.teams = aEncounter.teams;
        return encounter;
    }

    public teamNames():Array<string>{
        return this.teams.map(t => t.name);
    }
    
    constructor(sportName:string, hasResult:boolean, date:Date){
        this.id = 0;
        this.sportName = sportName;
        this.hasResult = hasResult;
        this.date = date;
        this.teamIds = new Array<number>();
        this.commentsids = new Array<number>();
        this.teams = new Array<Team>();
    }

}