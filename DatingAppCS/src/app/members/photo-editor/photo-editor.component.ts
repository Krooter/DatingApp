import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getPhotoChange = new EventEmitter<string>();

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  hasAnotherDropZoneOver: boolean;
  response: string;
  baseUrl = environment.apiUrl;
  currentMain: Photo;
  basePhotoUrl = '../../assets/user.png';

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) {
    this.uploader = new FileUploader({
      url: this.baseUrl,
      disableMultipart: true, // 'DisableMultipart' must be 'true' for formatDataFunction to be called.
      formatDataFunctionIsAsync: true,
      formatDataFunction: async (item) => {
        return new Promise( (resolve, reject) => {
          resolve({
            name: item._file.name,
            length: item._file.size,
            contentType: item._file.type,
            date: new Date()
          });
        });
      }
    });

    this.hasBaseDropZoneOver = false;

    this.response = '';

    this.uploader.response.subscribe( res => this.response = res );
  }

  ngOnInit() {
    this.initialiseUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initialiseUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, respone, status, headers) => {
      if(respone){
        const res: Photo = JSON.parse(respone);
        const photo = {
          id: res.id,
          urlPhoto: res.urlPhoto,
          description: res.description,
          dateAdded: res.dateAdded,
          isMain: res.isMain,
          urlPhotoAvatar: res.urlPhotoAvatar
        };
        this.photos.push(photo);
        if (photo.isMain){
          this.authService.changeMemberPhoto(photo.urlPhotoAvatar);
          this.authService.currentUser.photoUrlAvatar = photo.urlPhotoAvatar;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    };
  }

  setMain(photo: Photo){
    if (this.photos.filter(p => p.isMain === true)[0] != null){
      this.userService.setMain(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
        this.currentMain = this.photos.filter(p => p.isMain === true)[0];
        this.currentMain.isMain = false;
        photo.isMain = true;
        this.authService.changeMemberPhoto(photo.urlPhotoAvatar);
        this.authService.currentUser.photoUrl = photo.urlPhotoAvatar;
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        this.alertify.success('Succsefully set to main');
      }, error => {
        this.alertify.error(error);
    });
  } else {
        this.userService.setMain(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
        this.currentMain = this.photos.filter(p => p.id === photo.id)[0];
        this.currentMain.isMain = true;
        this.authService.changeMemberPhoto(this.currentMain.urlPhotoAvatar);
        this.authService.currentUser.photoUrl = this.currentMain.urlPhotoAvatar;
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        this.alertify.success('Succsefully set to main');
      }, error => {
        this.alertify.error(error);
      });
    }
  }

  deletePhoto(id: number){
    console.log(this.photos.findIndex(p => p.id === id && p.isMain === true));
    if (this.photos.find(p => p.id === id).isMain === false){
        this.alertify.confirm('Are you sure you want to delete this photo?', () => {
          this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
            this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
            localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
            this.alertify.success('Photo has been deleted');
          }, error => {
            this.alertify.error(error);
          });
        });
      } else if (this.photos.find(p => p.id === id).isMain === true){
        this.alertify.confirm('Are you sure you want to delete this photo?', () => {
          this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
            this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
            this.authService.changeMemberPhoto(this.basePhotoUrl);
            this.authService.currentUser.photoUrl = this.basePhotoUrl;
            localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
            this.alertify.success('Photo has been deleted');
          }, error => {
            this.alertify.error(error);
          });
        });
    }
  }
}
