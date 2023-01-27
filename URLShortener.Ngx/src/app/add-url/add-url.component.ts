import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-url',
  templateUrl: './add-url.component.html',
  styleUrls: ['./add-url.component.scss'],
})
export class AddUrlComponent {
  originalUrl = new FormControl('', [
    Validators.required,
    Validators.pattern('https?://.+'),
  ]);

  getErrorMessage() {
    if (this.originalUrl!.hasError('required')) {
      return 'You must enter a value';
    }

    return this.originalUrl!.hasError('pattern') ? 'Not a valid URL' : '';
  }
}
