//Problem: Hints are shown even when form is valid
//Solution: Hide and show them at appropriate times
var password = document.getElementById('Password');
var confirmPassword = document.getElementById('PasswordConfirm');
var span = document.getElementsByTagName('span');

//Hide hints
span.style.display = 'none';

function isPasswordValid() {
	return password.val().length > 6;
}

function arePasswordsMatching() {
	return password.val() === confirmPassword.val();
}

function canSubmit() {
	return arePasswordsMatching();
}

function passwordEvent() {
	//Find out if password is valid  
	if (isPasswordValid()) {
		//Hide hint if valid
		password.next().hide();
	} else {
		//else show hint
		password.next().show();
	}
}

function confirmPasswordEvent() {
	//Find out if password and confirmation match
	if (arePasswordsMatching()) {
		//Hide hint if match
		confirmPassword.next().hide();
	} else {
		//else show hint 
		confirmPassword.next().show();
	}
}

function enableSubmitEvent() {
    document.getElementById('submit').prop("disabled", !canSubmit());
}

//When event happens on password input
password.focus(passwordEvent).keyup(passwordEvent).keyup(confirmPasswordEvent).keyup(enableSubmitEvent);

//When event happens on confirmation input
confirmPassword.focus(confirmPasswordEvent).keyup(confirmPasswordEvent).keyup(enableSubmitEvent);

enableSubmitEvent();