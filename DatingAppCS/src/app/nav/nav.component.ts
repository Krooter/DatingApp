import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }


  // tslint:disable: typedef
  ngOnInit() {

  }

  login(){
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in succesful!');
    }, error => {
      console.log('Login failed!');
    });
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    return !!token;
  }

  loggedOut(){
    localStorage.removeItem('token');
    console.log('logged out');
  }
}
