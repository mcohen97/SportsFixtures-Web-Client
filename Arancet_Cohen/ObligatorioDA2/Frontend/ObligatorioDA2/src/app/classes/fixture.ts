export class Fixture{
    fixtureName:string;
    day:number;
    month:number;
    year:number;
    sportName:string;
    
    constructor(fixtureName:string, day:number, month:number, year:number, sportName:string){
        this.fixtureName = fixtureName;
        this.day = day;
        this.month = month;
        this.year = year;
        this.sportName = sportName;
    }

    public static Clone(aFixture:Fixture){
        return new Fixture(aFixture.fixtureName, aFixture.day, aFixture.month, aFixture.year, aFixture.sportName);
    }
}