// 8x8 names can be randomly generated
const first = ['Black-Wing', 'Soul-Growler', 'Spirit-Eye', 'Mold-Pox', 'Wind-Hack', 'Foul-Grin', 'Bone-Fist', 'Cold-Thorn'];
const second = ['Wraith', 'Flayer', 'Grim', 'Hunter', 'Hungry', 'Sharp', 'Destroyer', 'Hammer'];

function generateName() {
    return `${first[Math.floor(Math.random() * 8)]} the ${second[Math.floor(Math.random() * 8)]}`;
}

export default generateName;