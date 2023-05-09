mergeInto(LibraryManager.library, {

    AdsFullExtern: function () {
        if (!ysdk || !ysdk.adv)
            return;

        try {
            ysdk
            .adv
            .showFullscreenAdv({
                callbacks: {
                    onClose: function(wasShown) {
                        myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds');
                    }
                }
            })

        } catch (err) {
            console.log('adv full exception');
            myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds') }

    },

    AdsActiveExtern: function (indexAward) {
        if (!ysdk || !ysdk.adv)
            return;

        try {
            ysdk
            .adv
            .showRewardedVideo({
                callbacks: {
                    onRewarded: () => {
                        myGameInstance.SendMessage('PublishHandler(Clone)', 'GetAward', indexAward);
                    },
                    onClose: () => {
                        myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds');  
                    }
                }
            })
        } catch (err) {
            console.log('rewarded video exception');
            myGameInstance.SendMessage('PublishHandler(Clone)', 'CloseAds') }
    },


    BuyGoodsExtern: function (idGoods) {
        if (!payments)
                return;

        var idString = UTF8ToString(idGoods);

        payments
        .purchase({ id: idString })
        .then(purchase => {
            myGameInstance.SendMessage('PublishHandler(Clone)', 'GetGoods', idString); })
        .catch(err => { console.log('buy goods exception') })
    },

    LoadDataExtern: function () {
        try {
            if (player && player.getMode() !== 'lite') {
                player
                .getData()
                .then(_data => {
                    const json = JSON.stringify(_data);
                    console.log(json);
                    myGameInstance.SendMessage('PublishHandler(Clone)', 'LoadData', json);
                })
            } else {
                myGameInstance.SendMessage('PublishHandler(Clone)', 'LoadData', 'local');
            }
        } catch (err) { 
            console.log('load data exception');
            myGameInstance.SendMessage('PublishHandler(Clone)', 'LoadData', 'local') }

    },

    SaveDataExtern: function (data) {
        if (!player)
                return;

        try {        
            var dataString = UTF8ToString(data);
            var json = JSON.parse(dataString);
            console.log(json);
            player.setData(json)
        } catch (err) { console.log('save data exception') }
    },

    CheckRateGame: function () {
        if (!ysdk || !ysdk.feedback)
            return;

        ysdk
        .feedback
        .canReview()
        .then(({ value, reason }) => {
            if (value) {
                myGameInstance.SendMessage('GameplayPanel(Clone)', 'OpenRateWindow');
            } })
        .catch(err => { console.log('check rate exception') })
    },

    RateGame: function () {
        if (!ysdk || !ysdk.feedback)
            return;

        ysdk
        .feedback
        .requestReview()
        .then(({ feedbackSent }) => { console.log(feedbackSent) })
        .catch(err => { console.log('rate game exception') })
    },

    SetLeaderBoard: function (value) {
        if (!ysdk || !player || player.getMode() === 'lite')
            return;

        try {
            ysdk
            .getLeaderboards()
            .then(lb => {
                lb.setLeaderboardScore('Leaders', value);
            })
        } catch (err) { console.log('set leaderboard exception') }
    }
});