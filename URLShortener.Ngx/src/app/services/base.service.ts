import { Observable } from 'rxjs';

export abstract class BaseService {
    constructor() { }

    protected handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            // TODO: send the error to remote logging infrastructure
            // log to console instead
            console.error(error); 
            throw error;
        };
    }

    protected log(message: string) {
        console.log(message);
    }
}
