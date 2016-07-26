var shapes = ["heater", "cartouche", "swiss", "iberian", "lozenge", "none"];
var currentShape = 0;
var shapeLoop = function(currentShape) {
  return shapes[Math.abs(currentShape % shapes.length)];
}

$(".shape").addClass(shapes[0]);

$(".shape-right").click(function(){
  $(".shape").removeClass(shapeLoop(currentShape));
  currentShape++;
  $(".shape").addClass(shapeLoop(currentShape));
})

$(".shape-left").click(function(){
  if(currentShape === 0) {
    currentShape = shapes.length;
  }

  $(".shape").removeClass(shapeLoop(currentShape));
  currentShape--;
  $(".shape").addClass(shapeLoop(currentShape));
})
