(function () {
	try {

		Object.defineProperty(console, '_commandLineAPI',
			{
				get: function () {
					throw 'Nooo!'
				}
			})
		console.log("%cSTOP IT", "font: 2em sans-serif; color: yellow; background-color: red;");





	} catch (e) {
	}
})();