import { FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { UserError } from './userError';

export class CustomValidators extends Validators {
  
    static usernameAlreadyExist(userError:UserError): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } | null => {
          if (userError.errorMessage != undefined && userError.errorMessage != "") {
            return { usernameError: true };
          }else{
            return null;
          }
       };
    }
}