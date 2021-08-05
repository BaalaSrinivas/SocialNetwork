import { Component, OnInit } from '@angular/core';
import { Friend } from '../models/friend.model';
import { FollowService } from '../services/follow.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {

  friends: Friend[];

  pendingRequests: Friend[];

  constructor(private _followService: FollowService) { }

  ngOnInit(): void {

    this._followService.getFriends().subscribe(data => {
      this.friends = data;
    });

    this._followService.getFriendRequests().subscribe(data => {

      this.pendingRequests = data;

    });
  }

  acceptFriendRequest(friend: Friend) {
    this._followService.acceptFriendRequest(friend).subscribe(data => {

      console.log(data);

      this._followService.getFriends().subscribe(data => {
        this.friends = data;
      });

      this._followService.getFriendRequests().subscribe(data => {

        this.pendingRequests = data;
        console.log(this.pendingRequests);
      });
    });
  }

  rejectFriendRequest(friend: Friend) {
    this._followService.deleteFriendRequest(friend).subscribe(data => {
      this._followService.getFriendRequests().subscribe(data => {
        this.pendingRequests = data;
      });
    });
  }

}
