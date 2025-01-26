shader_type canvas_item;

uniform vec4 toxic_color : source_color = vec4(0.2, 0.8, 0.1, 0.9);
uniform vec4 bubble_color : source_color = vec4(0.3, 0.9, 0.2, 0.6);
uniform float wave_speed = 1.5;
uniform float bubble_intensity = 0.03;

float random(vec2 uv) {
    return fract(sin(dot(uv.xy, vec2(12.9898,78.233))) * 43758.5453123);
}

void fragment() {
    vec2 uv = UV * 2.0 - 1.0;
    
    // Rounded/elliptical mask
    float ellipse = 1.0 - smoothstep(0.5, 1.0, length(vec2(uv.x, uv.y * 2.0)));
    
    float time = TIME * wave_speed;
    float bubbles = sin(uv.x * 20.0 + time * 2.0) * sin(uv.y * 20.0 + time) * bubble_intensity;
    float swirl = sin(uv.x * 8.0 + uv.y * 8.0 + time) * 0.02;
    
    // Rising bubbles
    float rising_bubbles = 0.0;
    for(float i = 0.0; i < 3.0; i++) {
        vec2 bubble_pos = vec2(
            sin(time * (0.5 + i * 0.2)) * 0.3,
            mod(time * (0.3 + i * 0.2) + i * 0.4, 2.0) - 1.0
        );
        rising_bubbles += smoothstep(0.05, 0.0, length(uv - bubble_pos));
    }
    
    vec2 distorted_uv = uv;
    distorted_uv.x += swirl + bubbles;
    distorted_uv.y += swirl + bubbles;
    
    COLOR = mix(toxic_color, bubble_color, bubbles * 5.0 + rising_bubbles);
    COLOR.a *= ellipse;
}