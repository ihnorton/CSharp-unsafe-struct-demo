pub fn sum(slice: &[f64]) -> f64 {
    return slice.iter().sum();
}



fn main() {
    println!("Hello, world!");
}


#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_sum() {
        let data: Vec<f64> = vec![1.0, 2.0, 3.5, 4.9];
        let total = sum(&data);

        assert_eq!(total, 11.4);
    }
}