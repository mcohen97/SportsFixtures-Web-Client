export class Fixture{
    fixtureName:string;
    sportName:string;
    initialDate:Date;
    
    constructor(fixtureName:string, date:Date , sportName:string){
        this.fixtureName = fixtureName;
        this.initialDate = date;
        this.sportName = sportName;
    }

    public static Clone(aFixture:Fixture){
        return new Fixture(aFixture.fixtureName, aFixture.initialDate, aFixture.sportName);
    }
}