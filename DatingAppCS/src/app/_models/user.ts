import { Photo } from './photo';

export interface User {
  id: number;
  username: string;
  knownAs: string;
  age: number;
  gender: string;
  created: Date;
  lastOnline: Date;
  photoUrl: string;
  photoUrlAvatar: string;
  city: string;
  country: string;
  interests?: string;
  instruction: string;
  lookingFor?: string;
  photos?: Photo[];
}
