
var id = 0;

id = $('table[id ^= "person"]').length - 1;


$('#add').click(function () {

    var index = new Number(id++);

    var newPerson = $('#person').clone().attr('id', 'person' + index);

    newPerson.find('#first').attr('id', 'first' + index).attr('name', 'first' + index);
    newPerson.find('#last').attr('id', 'last' + index).attr('name', 'last' + index);
    newPerson.find('#food').attr('id', 'food' + index).attr('name', 'food' + index);

    newPerson.find('#adult').attr('id', 'adult' + index).attr('name', 'age' + index);
    newPerson.find('#child').attr('id', 'child' + index).attr('name', 'age' + index);
    newPerson.find('#adultLabel').attr('id', 'adultLabel' + index).attr('for', 'adult' + index);
    newPerson.find('#childLabel').attr('id', 'childLabel' + index).attr('for', 'child' + index);

    newPerson.find('#yes').attr('id', 'yes' + index).attr('name', 'attending' + index);
    newPerson.find('#no').attr('id', 'no' + index).attr('name', 'attending' + index);
    newPerson.find('#yesLabel').attr('id', 'yesLabel' + index).attr('for', 'yes' + index);
    newPerson.find('#noLabel').attr('id', 'noLabel' + index).attr('for', 'no' + index);
    
    newPerson.find('#remove').attr('id', 'remove' + index);

    newPerson.show();

    newPerson.find('#remove' + index).click(function () { removePerson('#person' + index) });


    $('#add').before(newPerson);
});

function removePerson(name) {
        
    $(name).remove();
    
    var persons = $('table[id ^= "person"]');
    
    var newIndex = 0;
    
    var msg = '';
    
    persons.each(function(a) {
        
        var elementId = $(this).attr('id');
        
        var matches = elementId.match(/\d{1,}$/);
        
        if (matches)
        {  
            var oldIndex = matches[0];
                        
            var first = $(this).find('#first' + oldIndex);
            
            //msg += oldIndex + ' ' + first.attr('id') + ' ';
        
            $(this).find('#first' + oldIndex).attr('id', 'first' + newIndex).attr('name', 'first' + newIndex);
            $(this).find('#last' + oldIndex).attr('id', 'last' + newIndex).attr('name', 'last' + newIndex);
            $(this).find('#food' + oldIndex).attr('id', 'food' + newIndex).attr('name', 'food' + newIndex);
       
            $(this).find('#adult' + oldIndex).attr('id', 'adult' + newIndex).attr('name', 'age' + newIndex);
            $(this).find('#child' + oldIndex).attr('id', 'child' + newIndex).attr('name', 'age' + newIndex);       
            $(this).find('#adultLabel' + oldIndex).attr('id', 'adultLabel' + newIndex).attr('for', 'adult' + newIndex);
            $(this).find('#childLabel' + oldIndex).attr('id', 'childLabel' + newIndex).attr('for', 'child' + newIndex);
       
            $(this).find('#yes' + oldIndex).attr('id', 'yes' + newIndex).attr('name', 'attending' + newIndex);
            $(this).find('#no' + oldIndex).attr('id', 'no' + newIndex).attr('name', 'attending' + newIndex);       
            $(this).find('#yesLabel' + oldIndex).attr('id', 'yesLabel' + newIndex).attr('for', 'yes' + newIndex);
            $(this).find('#noLabel' + oldIndex).attr('id', 'noLabel' + newIndex).attr('for', 'no' + newIndex);
       
            $(this).find('#remove' + oldIndex).attr('id', 'remove' + newIndex);
            
            $(this).attr('id', '#person' + newIndex);
            
            //msg += id + ' ' + first.attr('id') + ' ';
        
            newIndex++;
        }
    });
}
