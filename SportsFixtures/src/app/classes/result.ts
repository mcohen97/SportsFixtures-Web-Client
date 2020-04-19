import { Standing } from "./standing";

export class Result {
    team_position: Array<Standing>;

    public static getClone(aResul:Result): Result{   
        var newResult = new Result();
        newResult.team_position = aResul.team_position;
        return newResult;
    }
    
    constructor(){
        this.team_position = new Array<Standing>();
    }

}
