import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryImage, NgxGalleryOptions, NgxGalleryAnimation, NgxGalleryImageSize, NgxGalleryThumbnailsComponent } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  NgxGalleryImageSize: NgxGalleryImageSize[];
  galleryThumbnail: NgxGalleryThumbnailsComponent[];

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  // tslint:disable: typedef
  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    this.galleryOptions = [
      {
        width: '700px',
        height: '1000px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Fade,
        preview: false,
        imageArrowsAutoHide: true,
        imageInfinityMove: true,
        imageSize: NgxGalleryImageSize.Contain,
        thumbnailSize: NgxGalleryImageSize.Contain,
        thumbnailsMargin: 10
      },
      {
        breakpoint: 1100,
        width: '100%',
        height: '1100px',
        imagePercent: 80,
        thumbnailsPercent: 10,
        thumbnailsMargin: 10,
        thumbnailMargin: 10,
        thumbnailSize: NgxGalleryImageSize.Contain
      },
      {
        thumbnailSize: NgxGalleryImageSize.Contain,
        breakpoint: 400,
        preview: true
      }
    ];

    this.galleryImages = this.getImages();
  }
  getImages() {
    const imageUrls = [];
    for (const photo of this.user.photos) {
      imageUrls.push({
        thumbnailSize: NgxGalleryImageSize.Contain,
        small: photo.urlPhoto,
        medium: photo.urlPhoto,
        big: photo.urlPhoto,
        description: photo.description,
      });
    }
    return imageUrls;
  }

  // loadUser() {
  //   // tslint:disable-next-line: no-string-literal
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) => {
  //     this.user = user;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }
}
