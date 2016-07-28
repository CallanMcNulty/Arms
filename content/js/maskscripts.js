var shapes = ["heater", "cartouche", "swiss", "iberian", "lozenge", "none"];
var currentShape = $("input#special-form").val();
var shapeLoop = function(currentShape) {
  return shapes[Math.abs(currentShape % shapes.length)];
}

$(".shape").addClass(shapes[currentShape]);
console.log("Current shape is " + currentShape);

$(".shape-right").click(function(){
  $(".shape").removeClass(shapeLoop(currentShape));
  currentShape++;
  $(".shape").addClass(shapeLoop(currentShape));
  $("input#special-form:text").val(Math.abs(currentShape % shapes.length));
})

$(".shape-left").click(function(){
  console.log(shapes);
  if(currentShape === 0) {
    currentShape = shapes.length;
    console.log(currentShape);
  }

  $(".shape").removeClass(shapeLoop(currentShape));
  console.log("Removed " + currentShape);
  currentShape--;
  console.log("Current shape is " + currentShape);
  $(".shape").addClass(shapeLoop(currentShape));
  console.log("Added " + currentShape);
  $("input#special-form:text").val(Math.abs(currentShape % shapes.length));
})

$("form#form1").submit(function(event){
  currentShape = $("input#special-form").val();
  $(".shape").addClass(shapes[currentShape]);
})

$('.option2').click(function(){
  $('.results').hide();
})

$('.option1').click(function(){
  $('.results').show();
})
