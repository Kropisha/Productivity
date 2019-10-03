(function($) {

	/* Select the checkbox */
	var checkList = $('.check'),
		toDoCheck = checkList.children('input[type="checkbox"]');

	/* Check out whether checkbox is checked, and add 'task-checked' class to
	  the closest <li> item
	*/
	toDoCheck.each(function(index, element) {
		var $this = $(element),
			taskItem = $this.closest('li');

		$this.on('click', function(e) {
			taskItem.toggleClass('task-checked');
		});
	});

});