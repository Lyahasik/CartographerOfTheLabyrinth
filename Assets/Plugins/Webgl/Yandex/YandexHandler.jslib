mergeInto(LibraryManager.library, {

  AdsBlock: function () {
    window.alert("Ads block");
},

CheckRateGame: function () {
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
        if (value) {
            myGameInstance.SendMessage('GameplayPanel(Clone)', 'OpenRateWindow');
            console.log("OpenRateWindow success");
        }
        else
        {
            console.log("OpenRateWindow fail");
        }
    })
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