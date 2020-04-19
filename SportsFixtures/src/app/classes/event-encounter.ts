import { Encounter } from "./encounter";

export class EventEncounters{
    date:Date;
    enocunters:Array<Encounter>;

    constructor(){
        this.date = new Date(Date.now());
        this.enocunters = new Array<Encounter>();
    }
}