$(document).ready(function() {
	//Top Menu
	topMenu();

	//Tabs 
	tabs();

	//Link Select
	inputShareLinkSelect();
});

function tabs() {
	$( '#share' ).hide();
	var a = $( '.tab-navigation a' );
	$( "a[href='#description']" ).addClass('current');

	var tab = $( '.tab' );
	a.click(function () {
		tab.hide();
		var id = $( this ).attr('href');
		a.removeClass('current');
		$( this ).addClass('current');
		$( id ).fadeIn();
		return false;
	});
}

function inputShareLinkSelect() {
	$( "a[href='#share']" ).click(function(){
		$( '#share input' ).select();
	});
} 

function topMenu() {
	var show_menu = $( '.show-menu' );
	var close_menu = $( '.close-menu' );
	var header = $( 'header' );

	show_menu.click(function() {
		header.animate({top:"0px"}, 200);
		show_menu.hide();
		close_menu.show();
	});

	close_menu.click(function() {
		header.animate({top:"-47px"}, 200);
		show_menu.show();
		close_menu.hide();
	});
}