using FollowService.Events.EventModel;
using FollowService.Models;
using FollowService.Repository;
using MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Events.EventHandler
{
    public class UserAddedEventHandler : IEventHandler<UserAddedEventModel>
    {
        private IUnitofWork _unitofWork;

        public UserAddedEventHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async void Handle(UserAddedEventModel message)
        {
            //TODO: To be called on listening to new user added event
            FollowMetaData followInfo = new FollowMetaData(message.UserId);
            await _unitofWork.FollowMetaDataRepository.AddItemAsync(followInfo);

            _unitofWork.Commit();
        }
    }
}
