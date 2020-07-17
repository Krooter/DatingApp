import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})

export class AlertifyService {

constructor() { }

// tslint:disable: typedef
confirm(message: string, okCallback: () => any){
  alertify.confirm(message, (e: any) => {
    if (e){
      okCallback();
    } else { }
  });
}

success(message: string) {
  alertify.set('notifier', 'position', 'top-center');
  alertify.success(message);
}

error(message: string) {
  alertify.set('notifier', 'position', 'top-center');
  alertify.error(message);
}

warning(message: string) {
  alertify.set('notifier', 'position', 'top-center');
  alertify.warning(message);
}

message(message: string) {
  alertify.set('notifier', 'position', 'top-center');
  alertify.message(message);
}

}
