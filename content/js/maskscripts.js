var shapes = ["heater", "cartouche", "swiss", "iberian", "lozenge"];
var currentShape = $("input#special-form").val();
for(i=0; i<shapes.length; i++)
{
  $(".shape").removeClass(shapes[i]);
}
$(".shape").addClass(shapes[currentShape]);

$(".shape-right").click(function(){
  currentShape++;
  if(currentShape>=shapes.length){
    currentShape = 0;
  }
  $(".special-form:text").val(currentShape);
  $("#form1").submit();
});

$(".shape-left").click(function(){
  currentShape--;
  if(currentShape<0){
    currentShape = shapes.length-1;
  }
  $(".special-form:text").val(currentShape);
  $("#form1").submit();
});

$('.hamburger').click(function(){
  $('.saved-results').slideDown('slow');
});
