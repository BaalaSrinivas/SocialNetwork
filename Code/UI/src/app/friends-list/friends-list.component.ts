import { Component, OnInit } from '@angular/core';
import { Friend } from '../profile/Models/friend.model';
import { FollowService } from '../shared/follow.service';

@Component({
    selector: 'app-friends-list',
    templateUrl: './friends-list.component.html',
    styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent implements OnInit {

    friends: Friend[];

    pendingRequests: Friend[];

    constructor(private _followService: FollowService) { }

    ngOnInit(): void {

        this._followService.getFriends().subscribe(data => {
            this.friends = data;
        });

        this._followService.getFriendRequests().subscribe(data => {

            this.pendingRequests = data;
            console.log(this.pendingRequests);
        });
    }

    acceptFriendRequest(friend: Friend) {
        friend.State = 1;
        this._followService.updateFriendRequest(friend).subscribe(data => {
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
        friend.State = 2;

        this._followService.updateFriendRequest(friend).subscribe(data => {
            this._followService.getFriendRequests().subscribe(data => {
                this.pendingRequests = data;
            });
        });
    }

}
