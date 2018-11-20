import { Team } from "./team";
import { Standing } from "./standing";

export class Encounter {
    id: number;
    sportName: string;
    hasResult: boolean;
    date: Date;
    teamIds: Array<number>;
    commentsids: Array<number>;
    teams: Array<Team>;
    team_Position: Array<Standing>;

    public static getClone(aEncounter:Encounter): Encounter{
        var encounter = new Encounter(aEncounter.sportName, aEncounter.hasResult, aEncounter.date);
        encounter.teamIds = aEncounter.teamIds;
        encounter.commentsids = aEncounter.commentsids;
        encounter.teams = aEncounter.teams;
        encounter.id = aEncounter.id;
        encounter.team_Position = aEncounter.team_Position;
        return encounter;
    }

    public static teamNames(encounter:Encounter):Array<string>{
        return encounter.teams.map(t => t.name);
    }
    
    constructor(sportName:string, hasResult:boolean, date:Date){
        this.id = 0;
        this.sportName = sportName;
        this.hasResult = hasResult;
        this.date = date;
        this.teamIds = new Array<number>();
        this.commentsids = new Array<number>();
        this.teams = new Array<Team>();
        this.team_Position = new Array<Standing>();
    }

}