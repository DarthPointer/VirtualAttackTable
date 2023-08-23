export function GetElementBoudingBox(element) {
    const boundingBox = element.getBoundingClientRect();

    return [boundingBox.width, boundingBox.height];
}