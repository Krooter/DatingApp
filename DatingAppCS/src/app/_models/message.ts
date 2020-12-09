export interface Message {
  id: number;
  senderId: number;
  senderKnownAs: string;
  senderUrlPhotoAvatar: string;
  recipientId: number;
  recipientKnownAs: string;
  recipientUrlPhotoAvatar: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
}
