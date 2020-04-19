export class Log {
    id:number;
    logType:string;
    message:string;
    username:string;
    date:Date;

    public static clone(log:Log):Log{
        var newLog = new Log(log.logType, log.message, log.username, log.date);
        newLog.id = log.id;
        return newLog;
    }

    constructor(logType:string, message:string, username:string, date:Date){
        this.logType = logType;
        this.message = message;
        this.username = username;
        this.date = date;
        this.id = 0;
    }
}