mergeInto(LibraryManager.library, {

    AdsFullExtern: function () {
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

    AdsActiveExtern: function (indexAward) {
        ysdk.adv.showRewardedVideo({
            callbacks: {
              onRewarded: () => {
                myGameInstance.SendMessage('PublishHandler(Clone)', 'GetAward', indexAward);
            },
            onClose: () => {
                myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds');  
            }
        }
    })
    },


    BuyGoodsExtern: function (idGoods) {
        var idString = UTF8ToString(idGoods);

        payments.purchase({ id: idString }).then(purchase => {
            myGameInstance.SendMessage('PublishHandler(Clone)', 'GetGoods', idString);
        }).catch(err => {})
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
            lb.setLeaderboardScore('Leaders', value);
        });
    }
});