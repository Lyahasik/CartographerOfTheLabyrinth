mergeInto(LibraryManager.library, {

    AdsFull: function (delay) {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function(wasShown) {
                    myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds');
                },
                onError: function(error) {
                    myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds');
                },
                onOffline: function() {
                    myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds');
                }
            }
        })
    },

    CheckRateGame: function () {
        ysdk.feedback.canReview()
        .then(({ value, reason }) => {
            if (value) {
                myGameInstance.SendMessage('GameplayPanel(Clone)', 'OpenRateWindow');
            }
        })
    },

    LoadDataExtern: function () {
        player.getData().then(_data => {
            const json = JSON.stringify(_data);
            console.log(json);
            myGameInstance.SendMessage('PublishHandler(Clone)', 'LoadData', json);
        })
    },

    SaveDataExtern: function (data) {
        var dataString = UTF8ToString(data);
        var json = JSON.parse(dataString);
        console.log(json);
        player.setData(json);
    },

    RateGame: function () {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
            console.log(feedbackSent);
        })
    },

    SetLeaderBoard: function (value) {
        ysdk.getLeaderboards()
        .then(lb => {
            lb.setLeaderboardScore('TestLeaders', value);
        });
    }
});