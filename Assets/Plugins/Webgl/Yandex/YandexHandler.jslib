mergeInto(LibraryManager.library, {

    AdsBlock: function () {
    // window.alert("Ads block");
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