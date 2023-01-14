/* Read struct from file and print it */

use std::fs::File;
use std::fs::Metadata;
use std::io::prelude::*;
use std::path::Path;
use std::str;

fn main() {
  let path = Path::new("/Users/inorton/git/playground/2022-1116-rustbyexample/demo_struct_io/test.bin");
  let display = path.display();

  let mut file = match File::open(&path) {
    Err(why) => panic!("couldn't open {}: {}", display, why),
    Ok(file) => file
  };

  let sz: usize = file.metadata().unwrap().len() as usize;
  let mut buffer = vec![0; sz];
  let sz_read: usize = file.read(&mut buffer).expect("failed to read!");

  print!("read: {}", sz_read);

  print!("  i32: {}", i32::from_ne_bytes(buffer[0..4].try_into().unwrap()));
  print!("  f64: {}", f64::from_ne_bytes(buffer[4..12].try_into().unwrap()));
  let str_size: u32 = u32::from_ne_bytes(buffer[12..16].try_into().unwrap());


  print!("  str: {:?}", str::from_utf8(buffer[16..(16 + str_size as usize)].try_into().unwrap()));
}