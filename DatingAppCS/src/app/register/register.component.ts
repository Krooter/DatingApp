import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  // tslint:disable: typedef
  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-default'
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(7), Validators.maxLength(64)]],
      confirmPassword: ['', Validators.required],
      gender: ['male'],
      knownAs: ['', Validators.required],
      dateBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
    }, {validators: this.passwordMatchValidator});
  }

  passwordMatchValidator(g: FormControl) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {'missmatch': true};
  }

  register() {
    if (this.registerForm.valid){
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration succesful');
      }, error => {
        console.log(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        });
      });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('canceled');
  }
}
